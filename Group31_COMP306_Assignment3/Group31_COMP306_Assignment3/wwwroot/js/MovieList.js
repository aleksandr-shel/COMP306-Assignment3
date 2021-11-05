


(function () {
    const movieListContainer = document.querySelector('.movie-list-container'),
        btnSort = document.querySelector('#sortByRating'),
        gridItems = document.querySelectorAll('.grid-item');
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
                    <div> <p> Rating: ${value.toFixed(2)}</p></div>
                    <div class="rate">
                        
                        <input type="radio" id="star5" name="rate" value="5" disabled/>
                        <label for="star5" title="text">5 stars</label>
                        <input type="radio" id="star4" name="rate" value="4" disabled/>
                        <label for="star4" title="text">4 stars</label>
                        <input type="radio" id="star3" name="rate" value="3" disabled/>
                        <label for="star3" title="text">3 stars</label>
                        <input type="radio" id="star2" name="rate" value="2" disabled/>
                        <label for="star2" title="text">2 stars</label>
                        <input type="radio" id="star1" name="rate" value="1" disabled/>
                        <label for="star1" title="text">1 star</label>
                    </div>
                    <a class="btn-outline-danger" href="/movie/delete/${key}">Delete</a>
                `;
                movieListContainer.appendChild(div);
                gridItems = document.querySelectorAll('.grid-item');
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
    console.log(gridItems);
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



}) ()