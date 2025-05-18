(function () {
  const toggleBtn = document.getElementById("themeToggle");
  const darkClass = "dark-mode";
  const storageKey = "site-theme";

  // Set initial state
  if (localStorage.getItem(storageKey) === "dark") {
    document.body.classList.add(darkClass);
    toggleBtn.classList.add("active");
  }

  toggleBtn.addEventListener("click", () => {
    const isDark = document.body.classList.toggle(darkClass);
    toggleBtn.classList.toggle("active");
    localStorage.setItem(storageKey, isDark ? "dark" : "light");
  });
})();
