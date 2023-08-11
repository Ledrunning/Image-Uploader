<template>
  <div v-if="visible" class="modal-container">
    <!-- Modal overlay -->
    <div class="modal-overlay"></div>

    <!-- Modal content -->
    <div class="modal-content">
      <h2>{{ modalTitle }}</h2>
      <div class="content">
        <slot></slot>
      </div>
      <div v-if="isConfirmation" class="actions">
        <button class="dialog-button" @click="confirmAction">
          {{ confirmText }}
        </button>
        <button class="dialog-button" @click="cancelAction">
          {{ cancelText }}
        </button>
      </div>
      <button class="dialog-button" @click="closeModal">Close</button>
    </div>
  </div>
</template>

<script lang="ts">
import "@/styles/custommodal.css";
import { ref, defineComponent, PropType } from "vue";

export default defineComponent({
  props: {
    visible: Boolean,
    isConfirmation: Boolean,
    modalTitle: String,
    confirmText: {
      type: String,
      default: "Confirm",
    },
    cancelText: {
      type: String,
      default: "Cancel",
    },
  },

  setup(props, { emit }) {
    const confirmAction = () => {
      emit("confirm");
    };

    const cancelAction = () => {
      emit("cancel");
    };

    const closeModal = () => {
      emit("close");
    };

    return {
      confirmAction,
      cancelAction,
      closeModal,
    };
  },
});
</script>
