document.addEventListener("DOMContentLoaded", function () {
    const enderecoAtual = window.location.pathname.toLowerCase();
    const linksNavbar = document.querySelectorAll('.navbar-nav .nav-link');

    for (const link of linksNavbar) {
        const atributoHref = link.getAttribute('href').toLowerCase();

        if (enderecoAtual === atributoHref || enderecoAtual.startsWith(atributoHref + "/")) {
            link.classList.remove('nav-link', 'text-primary');
            link.classList.add('btn', 'btn-primary', 'rounded-3');
        } else {
            link.classList.remove('btn', 'btn-primary', 'rounded-3');
            link.classList.add('nav-link', 'text-primary');
        }
    }
});