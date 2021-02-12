<script>
import { ref, computed } from 'vue';

export default {
  name: 'FancyComponent',
  setup() {
    const isModalOpened = ref(false);
    const toggleModal = () => {
      isModalOpened.value = ! isModalOpened.value;
    };
  
    const inputOne = ref(0);
    const inputTwo = ref(0);
    const result = computed(() => inputOne.value + inputTwo.value);
    const submitInputs = (one, two) => {
      inputOne.value = one;
      inputTwo.value = two;
    };

    const tableColumns = ['Name', 'Surname', 'Email', 'Actions'];

    return {
      isModalOpened,
      toggleModal,

      inputOne,
      inputTwo,
      result,
      submitInputs,

      tableColumns,
    };
  },
};
</script>
