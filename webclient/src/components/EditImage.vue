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
        <label class="buttons" @click="openImage" for="fileInput">Open</label>
        <button class="buttons" @click="saveImage">Save</button>
        <button class="buttons">Update</button>
        <button class="buttons">Delete</button>
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

import "@/styles/genstyle.css";
import "@/styles/editImage.css";
import IImageDto from "@/model/ImageDto";
import FileService from "@/services/FileService";

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
    const userService = new ImageApiService();
    const fileService = new FileService();

    onMounted(async () => {
      // Suppose fetchFileName is a method that gets the file name based on the id
      const fetchedData = await userService.getImage(id);
      fillData(fetchedData);
    });

    async function fillData(dto: IImageDto) {
      fileName.value = dto.name;
      idText.value = dto.id !== undefined ? dto.id?.toString() : "";
      dateTimeText.value = DateTimeHelper.formatDateToLocalString(dto.dateTime);
      createdTimeText.value = DateTimeHelper.formatDateToLocalString(
        dto.creationTime
      );
      fileSizeText.value = dto.fileSize.toString();

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

        reader.onload = (e) => {
          if (e.target && e.target.result) {
            imageSrc.value = e.target.result as string;
          }
        };

        reader.readAsDataURL(file);
      }
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
    };
  },
});
</script>
