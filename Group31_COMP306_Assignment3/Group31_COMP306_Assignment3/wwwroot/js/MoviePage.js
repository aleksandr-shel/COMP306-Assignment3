


(function () {

    let inputRating = document.querySelectorAll('input[type="radio"]');
    const movieTitle = document.querySelector("input[name='movieTitle']");
    const userId = document.querySelector("input[name='userId']");


    let res = fetch(`/rating/getrating?movieTitle=${movieTitle.value}&userId=${userId.value}`).then(response => {
        return response.json();
    })
    res.then(data => {
        inputRating.forEach(x => {
            if (x.value == data.value) {
                x.checked = true;
            }
        })
    });
})()
