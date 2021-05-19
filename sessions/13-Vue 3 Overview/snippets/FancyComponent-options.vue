<script>
export default {
  name: 'FancyComponent',
  data() {
    return {
      isModalOpened: false,
      inputOne: 0,
      inputTwo: 0,
      tableColumns: ['Name', 'Surname', 'Email', 'Actions'],
    };
  },
  computed: {
    result() {
      return this.inputOne + this.inputTwo;
    },
  },
  methods: {
    toggleModal() {
      this.isModalOpened = ! this.isModalOpened;
    },
    submitInputs(one, two) {
      this.inputOne = one;
      this.inputTwo = two;
    },
  },
};
</script>
