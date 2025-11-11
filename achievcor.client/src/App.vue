<template>
  <header>
    <Header />
  </header>

  <main>
    <!--<div class="sidebar">
      <Sidebar />
    </div>-->
    <div class="main-container">
      <!-- Main Content Area -->
      <div class="content-area">
        <div class="content-wrapper">
          <router-view v-slot="{ Component }">
            <transition name="page-fade" mode="out-in">
              <component :is="Component" />
            </transition>
          </router-view>
        </div>
      </div>
    </div>
  </main>
</template>


<script lang="ts">
  import { defineComponent } from "vue";
  import Header from './components/HeaderComponent.vue'
  import Sidebar from './components/SidebarComponent.vue'

  export default defineComponent({
    name: 'AppLayout',
    components: {
      
    },
  });
</script>

