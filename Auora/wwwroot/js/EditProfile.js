//Botoes modal

// salvar imagem da galeria
function saveImage(path) {

    document.getElementById("bttServerSave").addEventListener("click", function () {
        updateImageOnServer(path, false);
    })


    document.querySelectorAll(".image-grid img").forEach(img => {
        img.classList.remove("selected");
    });
    event.target.classList.add("selected");
}

//função upload do computador

document.getElementById("uploadInput").addEventListener("change", function () {
    const file = this.files[0];
    if (!file) return;

    document.getElementById("bttUploadSave").style.display = "block"

    const formData = new FormData();
    formData.append("file", file);

    const token = document.querySelector('input[name="__RequestVerificationToken"]').value;
    formData.append("__RequestVerificationToken", token);

    document.getElementById("bttUploadSave").addEventListener("click", function () {  
        fetch('/EditProfille?handler=UploadImage', {
            method: 'POST',
            body: formData
        })
            .then(r => {
                if (!r.ok) throw new Error("Erro ao enviar imagem");
                return r.json();
            })
            .then(data => {
                if (data.success) {
                    updateProfileImagePreview(data.path);
                    document.getElementById("imageStatus").innerText = "Imagem enviada e atualizada!";
                } else {
                    document.getElementById("imageStatus").innerText = "Erro: " + data.message;
                }
            })
            .catch(err => {
                console.error(err);
                document.getElementById("imageStatus").innerText = "Erro ao comunicar com o servidor.";
            });

    })
});

// função genérica para atualizar no servidor
function updateImageOnServer(path, fromUpload) {
    fetch('/EditProfille?handler=UpdateImage', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
        },
        body: JSON.stringify({ imgPath: path })
    })
        .then(r => {
            if (!r.ok) throw new Error("Erro ao salvar imagem");
            return r.json();
        })
        .then(data => {
            if (data.success) {
                updateProfileImagePreview(path);
                document.getElementById("imageStatus").innerText = fromUpload ? "Imagem enviada!" : "Imagem selecionada!";
            } else {
                document.getElementById("imageStatus").innerText = "Erro: " + data.message;
            }
        })
        .catch(err => {
            console.error(err);
            document.getElementById("imageStatus").innerText = "Erro ao comunicar com o servidor.";
        });
}

// atualizar imagem de perfil no DOM
function updateProfileImagePreview(path) {
    document.getElementById("profileImage").src = path + "?t=" + new Date().getTime();
}




function showDiv(id, btn) {
    document.getElementById("div1").style.display = id === "div1" ? "block" : "none";
    document.getElementById("div2").style.display = id === "div2" ? "block" : "none";

    document.querySelectorAll(".indicator").forEach(ind => ind.classList.remove("active"));

    btn.querySelector(".indicator").classList.add("active");
}




function inputsStatus(status) {
    const btn = document.getElementById("bttTrigger");
    const fieldset = document.getElementById("fieldsetStatus"); // agora sim
    const form = document.getElementById("editForm");
    if (status === 1) {

        fieldset.disabled = false;

        btn.disabled = true;

    } else if (status === 2) {
        form.reset();
        fieldset.disabled = true;
        btn.disabled = false;
    }
}



