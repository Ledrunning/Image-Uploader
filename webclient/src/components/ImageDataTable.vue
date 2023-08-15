<template>
  <div>
    <EasyDataTable
      show-index
      buttons-pagination
      :headers="headers"
      :items="items"
      @click-row="showRow"
      alternative
    />
  </div>
  <div>
    <CustomToast v-model:isOpen="toastIsOpen" v-model:message="toastMessage" />
  </div>
</template>

<script lang="ts">
import "@/styles/spinnerloader.css";

import { useRouter } from "vue-router";
import type { Header, Item, ClickRowArgument } from "vue3-easy-data-table";
import { defineComponent, ref, onMounted } from "vue";
import ImageApiService from "@/services/ImageService";
import DateTimeHelper from "@/helpers/DateTimeHelper";
import CustomToast from "@/components/CustomToast.vue";

export default defineComponent({
  components: {
    CustomToast,
  },
  setup() {
    const toastMessage = ref("");
    const toastIsOpen = ref(false);
    const router = useRouter();
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
      router.push({
        name: "editimage",
        params: { id: item.id },
      });
    };

    onMounted(async () => {
      try {
        const res = await userService.getAllImages();

        items.value = res.map((item) => ({
          ...item,
          dateTime: DateTimeHelper.formatDateToLocalString(item.dateTime),
          creationTime: DateTimeHelper.formatDateToLocalString(
            item.creationTime
          ),
          fileSize: item.fileSize.toFixed(3),
        }));
      } catch (error) {
        showToast("An error occurred while fetching images");
        console.error("Error fetching images:", error);
      }
    });

    async function showToast(text: string) {
      toastIsOpen.value = true;
      toastMessage.value = text;
      await DateTimeHelper.delay(DateTimeHelper.delayTimeout);
      toastIsOpen.value = false;
    }

    return {
      headers,
      items,
      toastMessage,
      toastIsOpen,
      showRow,
    };
  },
});
</script>
