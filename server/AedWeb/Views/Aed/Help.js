

var helptxt = [
    { head: 'Cim1', txt: '1-Lorem ipsum, or lipsum as it is sometimes known, is dummy text used in laying out print, graphic or web designs. The passage is attributed to an unknown typesetter in the 15th century ' },
    { head: 'Cim2', txt: '2-Lorem ipsum, or lipsum as it is sometimes known, is dummy text used in laying out print, graphic or web designs. The passage is attributed to an unknown typesetter in the 15th century ' },
    { head: 'Cim3', txt: '3-Lorem ipsum, or lipsum as it is sometimes known, is dummy text used in laying out print, graphic or web designs. The passage is attributed to an unknown typesetter in the 15th century ' },
    { head: 'Cim4', txt: '4-Lorem ipsum, or lipsum as it is sometimes known, is dummy text used in laying out print, graphic or web designs. The passage is attributed to an unknown typesetter in the 15th century ' },
    { head: 'Cim5', txt: '5-Lorem ipsum, or lipsum as it is sometimes known, is dummy text used in laying out print, graphic or web designs. The passage is attributed to an unknown typesetter in the 15th century ' },
    { head: 'Helyezze az ábrának megfelelően a bajbajutottra!', txt: 'Az elektródákat csupasz bőrfelületre kell helyezni! Egyiket a jobb kulcscsont alá, másikat a bal alsó-oldalsó bordaívreA ritmusanalízis során ellenőrzi a szívműködést, ha kamrafibrillációt érzékel, javasolja  a sokkot. Egyes készülékek érzékelik, ha rossz helyre helyeztük az elektródát, ez esetben utasítást ad, hogy ellenőrizzük az elektródák helyzetét.A készülékek az analízis során azt mondják, hogy ne nyúljunk a beteghez. Új irányelvek alapján azonban a ritmusanalízis alatt is folytatható a mellkas-kompresszió.' }
]

$('#photoGallery').jqxScrollView({ width: 600, height: 450, buttonsOffset: [0, 0], animationDuration: 1500, slideDuration: 4000 });
$('#StartBtn').jqxButton({ theme: pageVariable.Jqwtheme });
$('#StopBtn').jqxButton({ theme: pageVariable.Jqwtheme });
$('#StartBtn').click(function () {
    $('#photoGallery').jqxScrollView({ slideShow: true });
});
$('#StopBtn').click(function () {
    $('#photoGallery').jqxScrollView({ slideShow: false });
});


$('#photoGallery').bind('pageChanged', function (event) {
    var page = event.args.currentPage;
    $("#HlpHead").html(helptxt[page].head);
    $("#HlpTxt").html(helptxt[page].txt);

});


LoadComplettCallback();