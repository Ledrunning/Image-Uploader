<template>
  <div class="image-frame-container">
    <div class="image-frame">
      <img :src="imageSrc" class="image-size" alt="Description of the image" />
      <div v-if="loading" class="spinner"></div>
    </div>
    <div class="controls">
      <label class="labelz">File name</label>
      <input
        type="text"
        v-model="fileName"
        class="styled-textbox"
        placeholder="Enter text here"
      />
      <div class="buttons-container">
        <label class="buttons" for="fileInput">Open</label>
        <input
          type="file"
          @change="openImage"
          id="fileInput"
          ref="fileInput"
          class="input-file"
          accept="image/jpeg"
          hidden
        />
        <button class="buttons" @click="saveImage">Save</button>
        <button class="buttons" @click="updateImage">Update</button>
        <button class="buttons" @click="deleteImage">Delete</button>
      </div>
      <div class="labelz-container">
        <label class="labelz">Id: {{ idText }}</label>
        <label class="labelz">Added date and time: {{ dateTimeText }}</label>
        <label class="labelz">Creation time: {{ createdTimeText }}</label>
        <label class="labelz">File size, Mb: {{ fileSizeText }}</label>
      </div>
    </div>
    <div>
      <CustomToast
        v-model:isOpen="toastIsOpen"
        v-model:message="toastMessage"
      />
    </div>
    <CustomModal
      :visible="showConfirmModal"
      modalTitle="Image uploader"
      isConfirmation
      @confirm="handleConfirm"
      @cancel="handleCancel"
    >
      {{ modalText }}
    </CustomModal>
  </div>
</template>

<script lang="ts">
import "@/styles/genstyle.css";
import "@/styles/editImage.css";

import { ref, onMounted } from "vue";
import { useRoute } from "vue-router";
import { defineComponent } from "vue";
import CustomToast from "@/components/CustomToast.vue";
import CustomModal from "@/components/CustomModal.vue";
import ImageApiService from "@/services/ImageService";
import DateTimeHelper from "@/helpers/DateTimeHelper";
import { FileUpdate } from "@/enum/FileUpdate";
import IImageDto from "@/model/ImageDto";
import FileService from "@/services/FileService";
import { useRouter } from "vue-router";

export default defineComponent({
  components: {
    CustomToast,
    CustomModal,
  },
  setup() {
    const route = useRoute();
    const id = Number(route.params.id); // This is from grid
    const fileName = ref("");
    const idText = ref("");
    const dateTimeText = ref("");
    const createdTimeText = ref("");
    const fileSizeText = ref("");
    const imageSrc = ref("");
    const loading = ref(false);
    const toastMessage = ref("");
    const toastIsOpen = ref(false);
    const showConfirmModal = ref(false);
    const isConfirm = ref(false);
    const modalText = ref("");
    const imageService = new ImageApiService();
    const fileService = new FileService();
    const router = useRouter();
    var fileUpdate = FileUpdate.NoOperation;
    var loadedFileName = "";
    let lastFileName = "";
    let currentFile: File | null = null;
    let isFileOpened = false;
    let imageData: number[] | null = null;

    onMounted(async () => {
      try {
        const fetchedData = await imageService.getImage(id);
        loadedFileName = fetchedData.name;

        loading.value = true;
        fillData(fetchedData);
      } catch (error) {
        showToast("An error occurred while the image getting");
        console.log("Image getting error", error);
      } finally {
        loading.value = false;
      }
    });

    async function fillData(dto: IImageDto) {
      fileName.value = dto.name;
      idText.value = dto.id !== undefined ? dto.id?.toString() : "";
      dateTimeText.value = DateTimeHelper.formatDateToLocalString(dto.dateTime);
      createdTimeText.value = DateTimeHelper.formatDateToLocalString(
        dto.creationTime
      );
      fileSizeText.value = dto.fileSize.toFixed(3).toString();
      imageData = dto.photo;
      // Updating the image source
      imageSrc.value = `data:image/png;base64,${dto.photo}`;
      lastFileName = dto.name;
    }

    async function saveImage() {
      await fileService.saveImage(imageSrc.value, fileName.value);
    }

    function openImage(event: Event) {
      try {
        loading.value = true;
        const fileInput = event.target as HTMLInputElement;
        const file = fileInput.files?.[0];

        if (file) {
          const reader = new FileReader();
          fileUpdate = FileUpdate.DeleteAndSave;

          reader.onload = (e) => {
            if (e.target?.result) {
              imageSrc.value = e.target.result as string;
            }
          };
          reader.readAsDataURL(file);

          currentFile = file;
          isFileOpened = true;
        }
      } catch (error) {
        showToast("Edit page: An error occurs when opening a file");
        console.log("Edit page: An error occurs when opening a file", error);
      } finally {
        loading.value = false;
      }
    }

    async function deleteImage() {
      showConfirmationWindow("Are you sure you want to proceed?");
      try {
        if (isConfirm.value) {
          loading.value = true;
          await imageService.deleteImage(id);
          router.push({
            name: "home",
          });
        } else {
          return;
        }
      } catch (error) {
        showToast("Edit page: An error occurs when deleting the data");
        console.log("Edit page: An error occurs when deleting the data", error);
      } finally {
        isConfirm.value = false;
        loading.value = false;
      }
    }

    async function updateImage() {
      try {
        if (loadedFileName !== fileName.value) {
          fileUpdate = FileUpdate.Rewrite;
        }

        loading.value = true;

        let fileCreatedTime =
          isFileOpened && currentFile !== null
            ? new Date(currentFile.lastModified)
            : DateTimeHelper.convertStringToIso(createdTimeText.value);
        let buffer =
          isFileOpened && currentFile !== null
            ? Array.from(await fileService.fileToByteArray(currentFile))
            : imageData;

        let imageDto = await createImageDto(buffer, fileCreatedTime);
        console.log(imageDto);
        if (imageDto !== null) {
          await imageService.updateImage(imageDto);
          isFileOpened = false;
        }
      } catch (error) {
        showToast("Edit page: An error occurs when updating the data");
        console.log("Edit page: An error occurs when updating the data");
      } finally {
        loading.value = false;
      }
    }

    // Create DTO from File
    async function createImageDto(
      imageByteArray: number[] | null,
      fileCreatedTime: Date
    ) {
      return {
        id: id,
        name: fileName.value,
        lastPhotoName: lastFileName,
        dateTime: DateTimeHelper.convertStringToIso(dateTimeText.value),
        creationTime: fileCreatedTime,
        fileSize: isNaN(Number(fileSizeText.value))
          ? 0
          : Number(fileSizeText.value),
        photo: imageByteArray,
        fileUpdate: fileUpdate,
      } as IImageDto;
    }

    async function showToast(text: string) {
      toastIsOpen.value = true;
      toastMessage.value = text;
      await DateTimeHelper.delay(DateTimeHelper.delayTimeout);
      toastIsOpen.value = false;
    }

    function showConfirmationWindow(text: string) {
      isConfirm.value = true;
      modalText.value = text;
      showConfirmModal.value = true;
    }

    const handleConfirm = () => {
      isConfirm.value = true;
      showConfirmModal.value = false;
    };

    const handleCancel = () => {
      isConfirm.value = false;
      showConfirmModal.value = false;
    };

    return {
      fileName,
      idText,
      dateTimeText,
      createdTimeText,
      fileSizeText,
      imageSrc,
      loading,
      toastIsOpen,
      toastMessage,
      showConfirmModal,
      modalText,
      isConfirm,
      saveImage,
      openImage,
      deleteImage,
      updateImage,
      handleConfirm,
      handleCancel,
    };
  },
});
</script>
