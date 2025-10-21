<template>
  <!--TODO:сделать нормальное выпадение-->
  <!--<nav class="navbar" style="background-color: #9c9c9c;">
    <div class="container-fluid">
      <button class="navbar-toggler"
              type="button"
              data-bs-toggle="offcanvas"
              data-bs-target="#offcanvasSidebar"
              aria-controls="offcanvasSidebar"
              aria-label="Переключатель навигации">
        <span class="navbar-toggler-icon"></span>
        <span class="navbar-brand text-white">{{brandname}}</span>
      </button>
    </div>
  </nav>-->

  <div class="offcanvas offcanvas-start show"
       data-bs-scroll="true"
       data-bs-backdrop="false"
       tabindex="-1"
       id="offcanvasSidebar"
       aria-labelledby="offcanvasLabel"
       style="background-color: #9c9c9c">
    <div class="offcanvas-body">
      <div class="p-4">
        <ul class="nav nav-pills flex-column">
          <li class="nav-item">
            <a class="nav-link active" aria-current="page" href="/">Главная</a>
          </li>
          <li class="nav-item">
            <a class="nav-link" aria-current="page" href="/profile">Профиль</a>
          </li>
          <li class="nav-item">
            <a class="nav-link" aria-current="page" href="#">Дела</a>
          </li>
          <li class="nav-item">
            <a class="nav-link" aria-current="page" href="/tasks">Задачи</a>
          </li>
        </ul>
      </div>
    </div>
  </div>
  <!--<div class="collapse collapse-horizontal" id="navbarToggleExternalContent">
    <div class="p-4" style="background-color: #9c9c9c;">
      <ul class="nav nav-pills flex-column">
        <li class="nav-item">
          <a class="nav-link active" aria-current="page" href="#">Активная</a>
        </li>
        <li class="nav-item">
          <a class="nav-link" href="#">Ссылка</a>
        </li>-->
  <!--<li class="nav-item">
    <a class="nav-link" href="#">Ссылка</a>
  </li>
  <li class="nav-item">
    <a class="nav-link disabled">Отключенная</a>
  </li>-->
  <!--</ul>
    </div>
  </div>-->

</template>

<script lang="ts">
    import { defineComponent } from 'vue';

    export default defineComponent({
        
    });
</script>

<style>
  .offcanvas.show {
    width: 260px;
  }

  /* Когда закрыт — сужаем и скрываем текст */
  .offcanvas:not(.show) {
    width: 70px !important;
  }

    .offcanvas:not(.show) .menu-text {
      display: none;
    }

    .offcanvas:not(.show) .menu-item {
      justify-content: center;
    }
</style>
