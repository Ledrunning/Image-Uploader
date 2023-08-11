<template>
  <div v-if="isOpen" class="toasty">
    {{ message }}
  </div>
</template>

<script lang="ts">
export default {
  props: {
    isOpen: Boolean,
    message: String,
  },
};
</script>

<style scoped>
.toasty {
  position: fixed;
  bottom: 20px;
  left: 50%;
  transform: translateX(-50%);
  background-color: #32b983;
  color: white;
  padding: 10px 20px;
  border-radius: 5px;
  z-index: 1000;
}
</style>
