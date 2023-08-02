<template>
  <div class="image-frame-container">
    <div class="image-frame">
      <img :src="imageSrc" class="image-size" alt="Description of the image" />
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
  </div>
</template>

<script lang="ts">
import { ref, onMounted } from "vue";
import { useRoute } from "vue-router";
import { defineComponent } from "vue";
import ImageApiService from "@/services/ImageService";
import DateTimeHelper from "@/helpers/DateTimeHelper";
import { FileUpdate } from "@/enum/FileUpdate";

import "@/styles/genstyle.css";
import "@/styles/editImage.css";
import IImageDto from "@/model/ImageDto";
import FileService from "@/services/FileService";
import { useRouter } from "vue-router";

export default defineComponent({
  setup() {
    const route = useRoute();
    const id = Number(route.params.id); // This is the id you passed
    const fileName = ref("");
    const idText = ref("");
    const dateTimeText = ref("");
    const createdTimeText = ref("");
    const fileSizeText = ref("");
    const imageSrc = ref("");
    const imageService = new ImageApiService();
    const fileService = new FileService();
    const router = useRouter();
    var fileUpdate = FileUpdate.NoOperation;
    var loadedFileName = "";
    let currentFile: File | null = null;

    onMounted(async () => {
      // Suppose fetchFileName is a method that gets the file name based on the id
      try {
        const fetchedData = await imageService.getImage(id);
        loadedFileName = fetchedData.name;
        fillData(fetchedData);
      } catch (error) {
        console.log("Image getting error", error);
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
      // Updating the image source
      imageSrc.value = `data:image/png;base64,${dto.photo}`;
    }

    async function saveImage() {
      await fileService.saveImage(imageSrc.value, fileName.value);
    }

    function openImage(event: Event) {
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
      }
    }

    async function deleteImage() {
      let result = window.confirm("Are you sure you want to proceed?");

      if (result) {
        await imageService.deleteImage(id);
        router.push({
          name: "home",
        });
        console.log("OK clicked");
      } else {
        console.log("Cancel clicked");
        return;
      }
    }

    async function updateImage() {
      if (loadedFileName !== fileName.value) {
        fileUpdate = FileUpdate.Rewrite;
      }
      if (!currentFile) {
        alert("Ups.. Something wrong with file loading!");
        return;
      }
      let imageDto = await createImageDto(currentFile, id);
      if (imageDto !== null) {
        await imageService.updateImage(imageDto);
      }
    }

    // Create DTO from File
    async function createImageDto(file: File, id: number) {
      const byteArray = await fileService.fileToByteArray(file);
      return {
        id: id,
        name: fileName.value,
        dateTime: DateTimeHelper.convertStringToIso(dateTimeText.value),
        creationTime: new Date(file.lastModified),
        fileSize: isNaN(Number(fileSizeText.value))
          ? 0
          : Number(fileSizeText.value),
        photo: Array.from(byteArray),
        fileUpdate: fileUpdate,
      } as IImageDto;
    }

    return {
      fileName,
      idText,
      dateTimeText,
      createdTimeText,
      fileSizeText,
      imageSrc,
      saveImage,
      openImage,
      deleteImage,
      updateImage,
    };
  },
});
</script>
