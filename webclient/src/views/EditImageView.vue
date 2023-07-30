<template>
  <div class="editImage" style=".//image">
    <EditImage msg="Edit your image data!" />
  </div>
</template>

<script lang="ts">
import { Options, Vue } from "vue-class-component";
import EditImage from "@/components/EditImage.vue";

@Options({
  components: {
    EditImage,
  },
})
export default class EditImageView extends Vue {}
</script>
