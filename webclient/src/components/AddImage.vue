<template>
  <div class="container">
    <div class="image-container">
      <img
        v-if="image"
        :src="image"
        alt="User selected image"
        class="uploaded-image"
      />
      <div v-if="loading" class="spinner spinner-centered"></div>
    </div>
    <div class="content">
      <label class="buttons" for="fileInput">Open</label>
      <input
        type="file"
        @change="onFileChange"
        id="fileInput"
        ref="fileInput"
        class="input-file"
        accept="image/jpeg"
        hidden
      />
      <button @click="deleteImage" class="buttons">Clear</button>
      <button @click="uploadImage" class="buttons">Add</button>
    </div>
    <div>
      <button @click="showModal">Show Modal</button>
      <CustomModal v-model:visible="modalVisible">
        <h2>My Modal Content</h2>
        <p>This is the content of the modal window.</p>
      </CustomModal>
    </div>
  </div>
</template>

<script lang="ts">
import "@/styles/addimage.css";
import "@/styles/genstyle.css";
import "@/styles/spinnerloader.css";
import { ref, defineComponent } from "vue";
import CustomModal from "@/components/CustomModal.vue";
import ImageApiService from "@/services/ImageService";
import IImageDto from "@/model/ImageDto";
import dayjs from "dayjs";
import utc from "dayjs/plugin/utc";
import timezone from "dayjs/plugin/timezone";
import { TimeType } from "@/enum/TimeType";
import DateTimeHelper from "@/helpers/DateTimeHelper";
import FileService from "@/services/FileService";
import { useRouter } from "vue-router";

export default defineComponent({
  components: {
    CustomModal,
  },
  setup() {
    let selectedFile: File | null = null;
    const fileService = new FileService();
    const userService = new ImageApiService();
    const modalVisible = ref(false);
    const image = ref("");
    const fileInput = ref(null);
    const loading = ref(false);
    const router = useRouter();
    dayjs.extend(utc);
    dayjs.extend(timezone);

    function onFileChange(e: Event) {
      const files = (e.target as HTMLInputElement).files;
      if (!files || files.length === 0) return;
      selectedFile = files[0];
      createImage(selectedFile);
    }

    function createImage(file: File) {
      const reader = new FileReader();
      reader.onload = (e) => {
        image.value = e.target?.result as string;
      };
      reader.readAsDataURL(file);
    }

    function deleteImage() {
      image.value = "";
      if (fileInput.value) {
        (fileInput.value as HTMLInputElement).value = "";
      }
      selectedFile = null;
    }

    // Upload Image Function
    async function uploadImage() {
      if (!selectedFile) {
        alert("Please select a file first.");
        return;
      }

      try {
        const imageDto = await createImageDto(selectedFile);

        loading.value = true; // Start loading

        await userService.addImage(imageDto);
      } catch (error) {
        console.log(error);
      } finally {
        loading.value = false; // Stop loading
        router.push({
          name: "home",
        });
      }
    }

    // Create DTO from File
    async function createImageDto(file: File) {
      const byteArray = await fileService.fileToByteArray(file);
      return {
        name: `MyPhoto_${DateTimeHelper.getUtcDateTimeNow(
          TimeType.FileNameDateTime
        )}.jpg`,
        dateTime: dayjs().tz().toDate(),
        creationTime: new Date(file.lastModified),
        fileSize: byteArray.byteLength * FileService.byteToMegabyteCoefficient,
        photo: Array.from(byteArray),
      } as IImageDto;
    }

    const showModal = () => {
      modalVisible.value = true;
    };

    return {
      image,
      onFileChange,
      deleteImage,
      uploadImage,
      fileInput,
      loading,
      showModal,
      modalVisible,
    };
  },
});
</script>
