<template>
  <h1>Bank Account Balance</h1>
  <p>{{ accountInUSD }}</p>
</template>

<script>
export default {
  props: {
    accountBalance: {
      type: Number,
      required: true,
    },
  },
  computed: {
    accountInUSD() {
      return '$' + this.accountBalance;
    },
  },
};
</script>
