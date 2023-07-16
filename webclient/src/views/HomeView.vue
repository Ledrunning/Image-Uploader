<template>
  <div class="home">
    <img alt="Vue logo" src="../assets/logo.png" />
    <ImageDataTable msg="Welcome to Your Vue.js + TypeScript App" />
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
