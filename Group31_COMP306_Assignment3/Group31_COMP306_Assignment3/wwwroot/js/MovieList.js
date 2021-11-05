


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
                    <div> <p>${value.toFixed(2)}</p></div>
                    <a class="btn-outline-danger" href="/movie/delete/${key}">Delete</a>
                `;
                movieListContainer.appendChild(div)
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


}) ()