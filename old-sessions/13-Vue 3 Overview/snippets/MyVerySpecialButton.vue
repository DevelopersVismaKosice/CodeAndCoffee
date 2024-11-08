<template>
  <button @click="increment">
    You clicked me {{ counter }} times.
  </button>
</template>

<script>
export default {
  name: 'MyVerySpecialButton',
  props: {
    title: {
      type: String,
      default: () => "Moj titulok",
    },
  },
  data() {
    return {
      counter: 0,
    };
  },
  methods: {
    increment() {
      this.counter++;
    },
  },
};
</script>


