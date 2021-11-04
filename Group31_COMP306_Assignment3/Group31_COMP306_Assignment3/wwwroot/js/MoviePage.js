


(function () {

    let inputRating = document.querySelectorAll('input[type="radio"]');
    const movieTitle = document.querySelector("input[name='movieTitle']");
    const userId = document.querySelector("input[name='userId']");

    inputRating.forEach(x => {
        console.log(x);
    });

    


    fetch("/rating/getrating")

})()
