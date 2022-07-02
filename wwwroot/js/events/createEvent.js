$('.carousel').carousel();
$('.carousel-inner > .carousel-item:first-child').addClass('active');

$('.event__top-download-link').on('change', function (e) {

    $('.event__carousel-inner').children('.carousel-item').remove();
    var files = e.target.files;
    handleFiles(files, $('.event__carousel-inner'));
    $('.carousel-inner > .carousel-item:first-child').addClass('active');
});

function handleFiles(files, innerContainer) {
    for (var i = 0; i < files.length; i++) {
        var file = files[i];

        if (!file.type.startsWith('image/')) { continue }

        var carouselItem = document.createElement("div");
        $(carouselItem).addClass('carousel-item');
        var img = document.createElement("img");
        $(img).addClass("w-100");
        img.file = file;
        carouselItem.appendChild(img);

        innerContainer.append(carouselItem);

        var reader = new FileReader();
        reader.onload = function (aImg) {
            return function (e) {
                aImg.src = e.target.result;
            };
        }(img);
        reader.readAsDataURL(file);
    }
};