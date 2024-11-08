<script>
import { ref, computed } from 'vue';
import { modalControls } from 'utils';

export default {
  name: 'FancyComponent',
  setup() {
    const inputOne = ref(0);
    const inputTwo = ref(0);
    const result = computed(() => inputOne.value + inputTwo.value);
    const submitInputs = (one, two) => {
      inputOne.value = one;
      inputTwo.value = two;
    };

    const tableColumns = ['Name', 'Surname', 'Email', 'Actions'];

    return {
      ...modalControls,

      inputOne,
      inputTwo,
      result,
      submitInputs,

      tableColumns,
    };
  },
};
</script>
