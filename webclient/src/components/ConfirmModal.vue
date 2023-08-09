<template>
  <Modal :visible="isVisible" @update:visible="handleClose">
    <template #header>
      <h2>{{ title }}</h2>
    </template>
    <slot></slot>
    <template #footer>
      <button @click="handleClose">Close</button>
    </template>
  </Modal>
</template>

<script lang="ts">
import { defineComponent, PropType } from "vue";
import { Modal } from "vue3-modal";

export default defineComponent({
  name: "ConfirmModal",
  components: {
    Modal, // Register the Modal component
  },
  props: {
    isVisible: {
      type: Boolean,
      required: true,
    },
    title: {
      type: String,
      required: true,
    },
  },
  methods: {
    handleClose() {
      this.$emit("update:isVisible", false);
    },
  },
});
</script>
