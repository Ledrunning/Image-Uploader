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
import { defineComponent } from "vue";
import ImageApiService from "@/services/image-service";

export default defineComponent({
  setup() {
    const userService = new ImageApiService();

    const headers: Header[] = [
      { text: "Id", value: "id" },
      { text: "Name", value: "name" },
      { text: "Date and time", value: "datetime" },
      { text: "Created time", value: "createdtime" },
      { text: "File size, Mb", value: "filesize" },
    ];

    const items = userService.getAllImages();

    return {
      headers,
      items,
    };
  },
});
//     const items: Item[] = [
//       {
//         id: 1,
//         name: "GSW",
//         datetime: "11072023-22:19",
//         createdtime: "11072020-13:23",
//         filesize: 1.258,
//       },
//       {
//         id: 2,
//         name: "LAL",
//         datetime: "11072023-22:20",
//         createdtime: "11072020-13:23",
//         filesize: 0.75,
//       },
//       {
//         id: 3,
//         name: "BKN",
//         datetime: "11072023-22:21",
//         createdtime: "11072020-13:23",
//         filesize: 1.005,
//       },
//       {
//         id: 4,
//         name: "SSS",
//         datetime: "11072023-22:22",
//         createdtime: "11072020-13:23",
//         filesize: 2.35,
//       },
//     ];
//     return {
//       headers,
//       items,
//     };
//   },
// });
</script>
