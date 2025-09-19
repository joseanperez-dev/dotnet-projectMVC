function confirmSweet(question, path) {
    Swal.fire({
        title: question,
        icon: 'error',
        showDenyButton: true,
        showCancelButton: true,
        confirmButtonText: "Sí",
        confirmButtonColor: "#3085d6",
        cancelButtonText: "No",
        cancelButtonColor: "#d33"
    }).then((result) => {
        if (result.isConfirmed) {
            window.location=path
        }
    });
}