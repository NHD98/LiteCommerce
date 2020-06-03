const selectAll = (event) => {
    const elements = document.querySelectorAll("input[type='checkbox']");
    if (event.target.checked) {
        [...elements].forEach(e => {
            e.checked = true;
        });
    } else {
        [...elements].forEach(e => {
            e.checked = false;
        });
    }
}