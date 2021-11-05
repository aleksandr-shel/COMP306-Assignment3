


(function () {
    const movieListContainer = document.querySelector('.movie-list-container'),
        btnSort = document.querySelector('#sortByRating');
    let order = "ascending";


    btnSort.addEventListener('click', () => {
        order = order == "ascending" ? "descending" : "ascending";
        let res = fetch(`/movie/sortedlist?order=${order}`).then(response => response.json())
        btnSort.innerHTML = `Sort by Rating: ${order}`;
        res.then(data => {
            movieListContainer.innerHTML = "";
            for (const [key, value] of Object.entries(data.ratingsDict)) {
                const div = document.createElement('div');
                div.classList.add('grid-item');
                div.innerHTML = `
                    <a href="/Movie/Page?key=${key}">
                        <h4>${key}</h4>
                        <video>
                            <source src="https://moviescomp306.s3.ca-central-1.amazonaws.com/${key}#t=0.1">
                        </video>
                    </a>
                    <div> <p> Rating: ${ value != 0 ? value.toFixed(2) : ''}</p></div>
                    <div class="rate">
                        <input type="hidden" name="rating" value="${value.toFixed(0)}"/>
                        <input type="radio" id="star5 ${key}" name="rate ${key}" value="5" disabled/>
                        <label for="star5 ${key}" title="text">5 stars</label>
                        <input type="radio" id="star4 ${key}" name="rate ${key}" value="4" disabled/>
                        <label for="star4 ${key}" title="text">4 stars</label>
                        <input type="radio" id="star3 ${key}" name="rate ${key}" value="3" disabled/>
                        <label for="star3 ${key}" title="text">3 stars</label>
                        <input type="radio" id="star2 ${key}" name="rate ${key}" value="2" disabled/>
                        <label for="star2 ${key}" title="text">2 stars</label>
                        <input type="radio" id="star1 ${key}" name="rate ${key}" value="1" disabled/>
                        <label for="star1 ${key}" title="text">1 star</label>
                    </div>
                `;
                movieListContainer.appendChild(div);
                putStars();
            }
        });
        //res.then(data => {
        //    movieListContainer.innerHTML = "";
        //    console.log(data);
        //    data.listOfMovies.forEach(x => {
        //        const div = document.createElement('div');
        //        div.classList.add('grid-item');
        //        div.innerHTML = `
        //            <a href="/Movie/Page?key=${x.key}">
        //                <h4>${x.key}</h4>
        //                <video>
        //                    <source src="https://moviescomp306.s3.ca-central-1.amazonaws.com/${x.key}#t=0.1">
        //                </video>
        //            </a>
        //            <div> <p>${data.ratingsDict[x.key].toFixed(2)}</p></div>
        //            <a class="btn-outline-danger" href="/movie/delete/${x.key}">Delete</a>
        //        `;
        //        movieListContainer.appendChild(div)
        //    });
        //});
    })
    function putStars() {
        gridItems = document.querySelectorAll('.grid-item');
        gridItems.forEach(gridItem => {
            const inputs = gridItem.querySelectorAll('.rate input[type="radio"]');
            const rating = gridItem.querySelector('input[name="rating"]');
            console.log(inputs);
            inputs.forEach(input => {
                if (input.value == rating.value) {
                    input.checked = true;
                }
            })
        })
    }

    putStars();

}) ()