//Mascara PhoneNumber and PostalCode
document.getElementById("phone").addEventListener("input", function () {
    let v = this.value.replace(/\D/g, ""); // somente n�meros
    if (v.length > 10) v = v.slice(0, 10); // m�ximo 10 d�gitos

    if (v.length > 6) {
        // (123) 456-7890
        v = v.replace(/^(\d{3})(\d{3})(\d{0,4})$/, "($1) $2-$3");
    } else if (v.length > 3) {
        // (123) 456
        v = v.replace(/^(\d{3})(\d{0,3})$/, "($1) $2");
    } else if (v.length > 0) {
        // (123
        v = v.replace(/^(\d{0,3})$/, "($1");
    }

    this.value = v;
});


document.getElementById("postal").addEventListener("input", function () {
    let v = this.value.replace(/\D/g, ""); // s� n�meros
    if (v.length > 9) v = v.slice(0, 9);   // m�ximo 9 d�gitos

    // Se houver mais de 5 d�gitos, aplica o h�fen para ZIP+4
    if (v.length > 5) {
        v = v.slice(0, 5) + "-" + v.slice(5);
    }

    this.value = v;
});




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

//fun��o upload do computador

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

// fun��o gen�rica para atualizar no servidor
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
    const fieldset = document.getElementById("fieldsetStatus");
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


