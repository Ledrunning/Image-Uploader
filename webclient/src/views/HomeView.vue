<template>
  <div class="home">
    <ImageDataTable msg="Welcome to Image Uploader app" />
  </div>
</template>

<script lang="ts">
import { Options, Vue } from "vue-class-component";
import ImageDataTable from "@/components/ImageDataTable.vue"; // @ is an alias to /src

@Options({
  components: {
    ImageDataTable,
  },
})
export default class HomeView extends Vue {}
</script>
