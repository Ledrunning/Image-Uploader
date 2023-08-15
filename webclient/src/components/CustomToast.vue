<template>
  <div v-if="isOpen" class="toasty">
    {{ message }}
  </div>
</template>

<script lang="ts">
import "@/styles/genstyle.css";

export default {
  props: {
    isOpen: Boolean,
    message: String,
  },
};
</script>
