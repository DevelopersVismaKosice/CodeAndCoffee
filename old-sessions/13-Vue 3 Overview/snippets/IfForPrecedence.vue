<template>
  <div>
    <!-- Both are NOT recommended to use! -->

    <!-- Precedence for Vue 2 -->
    <div v-for="element in list" v-if="bool" />

    <!-- Precedence for Vue 3 -->
    <div v-if="bool" v-for="element in list" />
  </div>
</template>

