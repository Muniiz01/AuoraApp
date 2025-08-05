


//Video player básico para a página inicial

document.querySelectorAll(".video-player").forEach((player) => {
    const video = player.querySelector("video");
    const toggleButton = player.querySelector(".toggleButton");

    if (!video || !toggleButton) {
        // Se faltar algo neste player, pula.
        return;
    }

    function togglePlay(e) {
        // Previna eventos de borbulhar se não quiser pausar/retomar outros players
        e?.stopPropagation?.();
        if (video.paused || video.ended) {
            video.play();
        } else {
            video.pause();
        }
    }

    function updateToggleButton() {
        toggleButton.textContent = (video.paused || video.ended) ? "►" : "❚❚";
    }

    // Liga eventos
    toggleButton.addEventListener("click", togglePlay);
    video.addEventListener("click", togglePlay);
    video.addEventListener("play", updateToggleButton);
    video.addEventListener("pause", updateToggleButton);
    video.addEventListener("ended", updateToggleButton); // garante reset quando termina

    // Estado inicial
    updateToggleButton();
});

// Aciona div que indica a pagina atual

document.addEventListener("DOMContentLoaded", () => {
    const btnActv = document.getElementById("secao1");
    btnActv.style.display = "block"; // Mostra a primeira seção por padrão
});


// Script carousel

    const carousel = document.querySelector('#multiItemCarousel');
    let isDown = false;
    let startX;
    let scrollLeft;

carousel.addEventListener('mousedown', (e) => {
        isDown = true;
    startX = e.pageX;
});

carousel.addEventListener('mouseup', (e) => {
        isDown = false;
});

carousel.addEventListener('mouseleave', () => {
        isDown = false;
});

carousel.addEventListener('mousemove', (e) => {
  if (!isDown) return;
    const walk = e.pageX - startX;
  if (walk > 50) { // arrastando para a direita
    const prev = bootstrap.Carousel.getInstance(carousel);
    prev.prev();
    isDown = false;
  } else if (walk < -50) { // arrastando para a esquerda
    const next = bootstrap.Carousel.getInstance(carousel);
    next.next();
    isDown = false;
  }
});

