import { ref, computed } from 'vue';

export const modalControls = () => {
  const isModalOpened = ref(false);
  const toggleModal = () => {
    isModalOpened.value = ! isModalOpened.value;
  };

  return {
    isModalOpened,
    toggleModal,
  };
};
