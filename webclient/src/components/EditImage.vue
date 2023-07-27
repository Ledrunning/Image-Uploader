<template>
  <div class="image-frame-container">
    <div class="image-frame">
      <img alt="Description of the image" />
    </div>
    <div class="controls">
      <label>File name:</label>
      <input type="text" class="styled-textbox" placeholder="Enter text here" />
      <div class="buttons-container">
        <button class="buttons">Open</button>
        <button class="buttons">Save</button>
        <button class="buttons">Update</button>
        <button class="buttons">Delete</button>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { useRoute } from "vue-router";
import { defineComponent } from "vue";
import "@/styles/genstyle.css";
import "@/styles/editImage.css";

export default defineComponent({
  setup() {
    const route = useRoute();
    const id = route.params.id; // This is the id you passed

    // Now you can fetch the item with this id
    // or if the item data is already in a global state, access it from there
  },
});
</script>
