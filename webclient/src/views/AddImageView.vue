<template>
  <div class="addImage">
    <AddImage msg="Add your image file!" />
  </div>
</template>

<script lang="ts">
import { Options, Vue } from "vue-class-component";
import AddImage from "@/components/AddImage.vue"; // @ is an alias to /src

@Options({
  components: {
    AddImage,
  },
})
export default class AddImageView extends Vue {}
</script>
