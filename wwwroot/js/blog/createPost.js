$('.carousel').carousel();
$('.carousel-inner > .carousel-item:first-child').addClass('active');

$('.post__body-mainSliderFiles').on('change', function (e) {

    $('.postCreate_mainSliderContainer-inner').children('.carousel-item').remove();
    var files = e.target.files;
    handleFiles(files, $('.postCreate_mainSliderContainer-inner'));
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

var indexTag = 0;
/*TagAdd*/
$('.postCreate-form__top-tags-add').click(function () {
    indexTag++;

    var tagItem = document.createElement('div');
    $(tagItem).addClass('col-3 m-2 tags-content__item d-flex align-items-center');

    var tag = document.createElement('input');
    tag.name = 'Tags[' + indexTag + '].Name';
    tag.type = 'text';
    $(tag).addClass('form-control input-primary-eco tags-content__item-tag');
    $(tagItem).append(tag);

    var deleteBtn = document.createElement('span');
    $(deleteBtn).addClass('tags-content__item-delete');
    $(tagItem).append(deleteBtn);

    $('.tags-content')[0].append(tagItem);
});

/*----RemoveTag ---------*/
$('.tags-content').click(function (e) {
    if ($(e.target).hasClass('tags-content__item-delete')) {
        $(e.target).parent().remove();
    }
});

/*----Add subtext ----------*/
var indexSubtext = 0;

$('.post__subtexts-addItem').click(function (e) {
    var postParticalIndex = e.target.dataset.partical;
    indexSubtext++;

    var subTextItem = document.createElement('div');
    $(subTextItem).addClass('post__subtexts-item subtext');

    var subTextTitleItem = document.createElement('div');
    $(subTextTitleItem).addClass('form-group subtext__title col-6');

    var subTextlabelTitle = document.createElement('label');
    subTextlabelTitle.innerText = 'Заголовок';
    $(subTextlabelTitle).addClass('subtext__title-label');

    var subTextTitle = document.createElement('input');
    $(subTextTitle).addClass('form-control');
    subTextTitle.name = 'PostParticals[' + postParticalIndex + '].SubTexts[' + indexSubtext + '].Title';

    $(subTextTitleItem).append(subTextlabelTitle);
    $(subTextTitleItem).append(subTextTitle);

    $(subTextItem).append(subTextTitleItem);

    var subTextContent = document.createElement('div');
    $(subTextContent).addClass('form-group subtext__content col-12 mt-2');
    var labelText = document.createElement('label');
    labelText.innerText = 'Текст'
    var textArea = document.createElement('textarea');
    textArea.name = 'PostParticals[' + postParticalIndex + '].SubTexts[' + indexSubtext + '].Text';
    $(textArea).addClass('form-control');

    $(subTextContent).append(labelText);
    $(subTextContent).append(textArea);

    $(subTextItem).append(subTextContent);

    $('.post__subtexts-content_' + postParticalIndex).append(subTextItem);
});


/*Remole Last subText*/
$('.post__subtexts-removeItem').click(function (e) {

    var postParticalIndex = e.target.dataset.partical;
    var particalContent = $('.post__subtexts-content_' + postParticalIndex);
    if (particalContent.children().length > 1) {
        particalContent.children().last().remove();
    }
});


/*particalSlider*/
$('.post__body-particalSlider').on('change', function (e) {
    var particalIndex = e.target.dataset.partical;
    var innderContainer = $('.carousel-inner_partical_'+particalIndex);
    var files = e.target.files;
    innderContainer.children().remove();
    handleFiles(files, $(innderContainer));
    innderContainer.children().first().addClass('active');
});
