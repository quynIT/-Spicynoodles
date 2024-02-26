const inputMaill = document.querySelector(".input-login-mail");
const inputPassword = document.querySelector(".input-login-password");
var inputUsernameRegister = document.querySelector(".input-signup-username");
const btnLogin = document.querySelector(".login_signInButton");

// validation form login

btnLogin.addEventListener("click", (e) => {
    if (inputMaill.value === "" || inputPassword.value === "") {
        alert("vui lòng không để trống");
    } else {
        const user = JSON.parse(localStorage.getItem(inputMaill.value));
        if (
            user.username === inputMaill.value &&
            user.password === inputPassword.value
        ) {
            alert("Đăng Nhập Thành Công");
            window.location.href = "~/Home/HomeClient";
        } else {
            TempData["error"] = "Tài khoảng đăng nhập không đúng";
        }
    }
});