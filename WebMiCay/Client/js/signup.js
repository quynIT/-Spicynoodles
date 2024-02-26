var inputUsernameRegister = document.querySelector(".input-signup-username");
var inputPasswordRegister = document.querySelector(".input-signup-password");
var inputPhone = document.querySelector(".input-signup-phone");
var inputName = document.querySelector(".input-signup-name");
var inputDate = document.querySelector(".input-signup-date");
var inputMap = document.querySelector(".input-signup-map");
var inputGioitinh = document.querySelector(".input-signup-sex");
var btnRegister = document.querySelector(".signup_signInButton");


 validation form register and register user local storage

btnRegister.addEventListener("click", (e) => {
    e.preventDefault();
    function validateForm() {
        if (inputUsernameRegister.value === "" ||
            inputPasswordRegister.value === "" ||
            inputPhone.value === "" ||
            inputName.value === "" ||
            inputDate.value === "" ||
            inputMap.value === "" ||
            inputGioitinh.value === ""
        ) {
            alert("Không được để trống");
            return false;
        } else if (!/(84|0[3|5|7|8|9])+([0-9]{8})\b/.test(inputPhone.value)) {
            alert("Sdt không đúng định dạng");
            return false;
        } else if (!/^[a-zA-ZÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂẾưăạảấầẩẫậắằẳẵặẹẻẽềềểếỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵỷỹ\s\W|_]+$/.test(inputUsernameRegister.value)) {
            alert("Tên không hợp lệ");
            return false;
        }
        return true;
    }
    if (validateForm()) {
        alert("Đăng Ký Thành Công");
        validateForm();
    }
});