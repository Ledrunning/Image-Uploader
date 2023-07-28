<template>
  <div class="image-frame-container">
    <div class="image-frame">
      <img alt="Description of the image" />
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
        <button class="buttons">Open</button>
        <button class="buttons">Save</button>
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

export default defineComponent({
  setup() {
    const route = useRoute();
    const id = Number(route.params.id); // This is the id you passed
    const fileName = ref("");
    const idText = ref("");
    const dateTimeText = ref("");
    const createdTimeText = ref("");
    const fileSizeText = ref("");
    const userService = new ImageApiService();
    // Now you can fetch the item with this id
    // or if the item data is already in a global state, access it from there

    onMounted(async () => {
      // Suppose fetchFileName is a method that gets the file name based on the id
      const fetchedData = await userService.getImage(id);
      fileName.value = fetchedData.name;
      idText.value = fetchedData.id!.toString();
      dateTimeText.value = DateTimeHelper.formatDateToLocalString(
        fetchedData.dateTime
      );
      createdTimeText.value = DateTimeHelper.formatDateToLocalString(
        fetchedData.creationTime
      );
      fileSizeText.value = fetchedData.fileSize.toString();
    });

    return {
      fileName,
      idText,
      dateTimeText,
      createdTimeText,
      fileSizeText,
    };
  },
});
</script>
