function fixGrammar(word) {
    var i = 0;
    while (word.includes("&#")) {

        console.log(word.includes("227"));
        if (word.includes("227")) { word = word.replace("&#227;", "ã"); }
        if (word.includes("231")) { word = word.replace("&#231;", "ç"); }
        if (word.includes("250")) { word = word.replace("&#250;", "ú"); }
        if (word.includes("233")) { word = word.replace("&#233;", "é"); }
        if (word.includes("193")) { word = word.replace("&#193;", "Á"); }
        if (word.includes("234")) { word = word.replace("&#234;", "ê"); }
        if (word.includes("243")) { word = word.replace("&#243;", "ó"); }


        i++;
        if (i > 30) { break;}
    }

    return word;
}