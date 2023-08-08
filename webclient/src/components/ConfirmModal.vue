<template>
  <v-modal :show="isOpen" @update:show="updateIsOpen">
    <div>
      <h1>{{ title }}</h1>
      <p>{{ message }}</p>
      <button @click="confirm">OK</button>
      <button @click="cancel">Cancel</button>
    </div>
  </v-modal>
</template>

<script lang="ts">
import { defineComponent, PropType } from "vue";

export default defineComponent({
  name: "ConfirmModal",
  props: {
    title: {
      type: String as PropType<string>,
      default: "",
    },
    message: {
      type: String as PropType<string>,
      default: "",
    },
    isOpen: {
      type: Boolean as PropType<boolean>,
      default: false,
    },
  },
  setup(props, { emit }) {
    const updateIsOpen = (value: boolean) => {
      emit("update:isOpen", value);
    };

    const confirm = () => {
      emit("confirm");
    };

    const cancel = () => {
      emit("cancel");
    };

    return { confirm, cancel, updateIsOpen };
  },
});
</script>
