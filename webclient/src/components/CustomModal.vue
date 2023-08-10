<template>
  <div class="modal-container" v-if="visible">
    <div class="modal-overlay"></div>
    <div class="modal-content">
      <button class="close-button" @click="closeModal">Close</button>
      <slot></slot>
    </div>
  </div>
</template>

<script lang="ts">
import "@/styles/custommodal.css";
import { defineComponent } from "vue";

export default defineComponent({
  props: {
    visible: {
      type: Boolean,
      required: true,
    },
  },
  emits: ["update:visible"],
  setup(props, { emit }) {
    const closeModal = () => {
      emit("update:visible", false);
    };

    return {
      closeModal,
    };
  },
});
</script>
