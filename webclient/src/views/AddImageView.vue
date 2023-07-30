<template>
  <div class="addImage" style=".//image">
    <AddImage msg="Add your image file!" />
  </div>
</template>

<script lang="ts">
import { Options, Vue } from "vue-class-component";
import AddImage from "@/components/AddImage.vue";

@Options({
  components: {
    AddImage,
  },
})
export default class AddImageView extends Vue {}
</script>
