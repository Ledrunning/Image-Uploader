<template>
  <div>
    <EasyDataTable
      buttons-pagination
      :headers="headers"
      :items="items"
      alternative
    />
  </div>
</template>

<script lang="ts">
import type { Header, Item } from "vue3-easy-data-table";
import { defineComponent, ref, onMounted } from "vue";
import ImageApiService from "@/services/image-service";

export default defineComponent({
  setup() {
    const userService = new ImageApiService();
    const headers: Header[] = [
      { text: "Id", value: "id" },
      { text: "Name", value: "name" },
      { text: "Date and time", value: "dateTime" },
      { text: "Created time", value: "createdTime" },
      { text: "File size, Mb", value: "fileSize" },
    ];

    const items = ref<Item[]>([]);

    onMounted(async () => {
      try {
        const res = await userService.getAllImages();
        items.value = res;
      } catch (error) {
        console.error("Error fetching images:", error);
      }
    });

    return {
      headers,
      items,
    };
  },
});
</script>
