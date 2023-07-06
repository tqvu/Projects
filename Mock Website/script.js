const wrapper = document.querySelector(".wrapper");
const about = document.querySelector(".about");
const contact = document.querySelector(".contact");
const loginLink = document.querySelector(".login-link");
const registerLink = document.querySelector(".register-link");
const buttonLogin = document.querySelector(".buttonLogin");
const iconClose = document.querySelector(".iconClose");
const buttonAbout = document.querySelector(".buttonAbout");
const aboutClose = document.querySelector(".aboutClose");
const buttonContact = document.querySelector(".buttonContact");
const contactClose = document.querySelector(".contactClose");
const buttonHome = document.querySelector(".buttonHome");



registerLink.addEventListener("click", ()=> {
    wrapper.classList.add("active");
});

loginLink.addEventListener("click", ()=> {
    wrapper.classList.remove("active");
});

buttonLogin.addEventListener("click", ()=> {
    wrapper.classList.add("active-popup");
    about.classList.remove("about-popup");
    contact.classList.remove("contact-popup");
});

iconClose.addEventListener("click", ()=> {
    wrapper.classList.remove("active-popup");
});

buttonAbout.addEventListener("click", ()=> {
    about.classList.add("about-popup");
    wrapper.classList.remove("active-popup");
    contact.classList.remove("contact-popup");
});

aboutClose.addEventListener("click", ()=> {
    about.classList.remove("about-popup");
});

buttonContact.addEventListener("click", ()=> {
    contact.classList.add("contact-popup");
    about.classList.remove("about-popup");
    wrapper.classList.remove("active-popup");
});

contactClose.addEventListener("click", ()=> {
    contact.classList.remove("contact-popup");
});

buttonHome.addEventListener("click", ()=> {
    contact.classList.remove("contact-popup");
    about.classList.remove("about-popup");
    wrapper.classList.remove("active-popup");
});

