<template>
  <div>
    <EasyDataTable
      v-model:items-selected="itemsSelected"
      show-index
      buttons-pagination
      :headers="headers"
      :items="items"
      @click-row="showRow"
      alternative
    />
    row clicked:<br />
    <div id="row-clicked"></div>
  </div>
</template>

<script lang="ts">
import type { Header, Item, ClickRowArgument } from "vue3-easy-data-table";
import { defineComponent, ref, onMounted } from "vue";
import ImageApiService from "@/services/image-service";
import dayjs from "dayjs";
import "@/styles/spinnerloader.css";

export default defineComponent({
  setup() {
    const userService = new ImageApiService();
    const headers: Header[] = [
      { text: "Id", value: "id" },
      { text: "Name", value: "name" },
      { text: "Date and time", value: "dateTime" },
      { text: "Created time", value: "creationTime" },
      { text: "File size, Mb", value: "fileSize" },
    ];

    const items = ref<Item[]>([]);

    const showRow = (item: ClickRowArgument) => {
      document!.getElementById("row-clicked")!.innerHTML = JSON.stringify(item);
      console.log("I've been clicked!");
    };

    onMounted(async () => {
      try {
        const res = await userService.getAllImages();

        items.value = res.map((item) => ({
          ...item,
          dateTime: dayjs(item.dateTime).format("YYYY-MM-DD HH:mm:ss"),
          creationTime: dayjs(item.creationTime).format("YYYY-MM-DD HH:mm:ss"),
        }));
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
