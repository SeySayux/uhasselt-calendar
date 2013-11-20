<?php
if($_GET['ajax'] != "1") {
?>
<!DOCTYPE html>
<html>
  <head>
    <meta http-equiv="content-type" content="text/html; charset=utf-8" />

    <title>UHasselt Calendar</title>

    <link rel="stylesheet" href="bootstrap/css/bootstrap.min.css"
        title="" type="text/css" />

    <script type="text/javascript" src="bootstrap/js/bootstrap.min.css"
        charset="utf-8">
    </script>

    <script type="text/javascript"
        src="http://codeorigin.jquery.com/jquery-2.0.3.min.js" charset="utf-8">

    </script>

    <script type="text/javascript" charset="utf-8"
        src="http://code.jquery.com/ui/1.10.3/jquery-ui.js">

    </script>

    <style type="text/css" media="all">

body {
    font-family: sans-serif;
    width: 640px;
    margin: 0 auto;
}

h1 {
    font-size: 300%;
    display: inline-block;
    width: 100%;
    text-align: center;
    text-shadow: 0px 1px silver;
}

h1 img {
    height: 128px;
    vertical-align: middle;
}

#name-input {
    display: block;
    width: 50%;
    margin: 0 auto;
    height: 40px;
    display: -webkit-box;
    display: -moz-box;
    display: -ms-flexbox;
    display: -webkit-flex;
    display: flex;
    align-items: baseline;
}

#name-input label {
    width: 50%;
    height: 20px;
    text-align: right;
}

.inputbox {
    border: 1px solid silver;
    border-radius: 5px;
}

.inputbox:hover {
    border: 1px solid grey;
}

#name-input input[type="text"] {
    margin-left: 30px;
    padding: 3px 5px;
}

.warningbox {
    border: 1px solid goldenrod;
    color: goldenrod;
    background-color: #ffffcc;
    padding: 5px 10px;
    margin: 5px auto;
    display: none;
}

.warningbox:before {
    display: inline-block;
    width: 16px;
    height: 16px;
    content: url("img/warning.png");
    vertical-align: -10%;
}

.errorbox {
    border: 1px solid maroon;
    color: maroon;
    background-color: #ffcccc;
    padding: 5px 10px;
    margin: 5px auto;
    display: none;
}

.errorbox:before {
    display: inline-block;
    width: 16px;
    height: 16px;
    content: url("img/error.png");
    vertical-align: -10%;
}

hr {
    margin-left: auto;
    margin-right: auto;
}

#textfield {
    display: block;
    margin: 0 auto;
}

textarea {
    display: block;
    width: 100%;
    box-sizing: border-box;
    height: 200px;
    font-family: monospace;
}

#textfield #load-file {
    margin: 5px;
}

.float-right {
    float: right;
    margin: 0px 2px;
}

.validation-message {
    display: none;
}

.validation-message.working {
    display: inline;
}

.validation-message.working:before {
    display: inline-block;
    width: 16px;
    height: 16px;
    content: url("img/throbber.gif");
    vertical-align: -15%;
    margin-left: 3px;
    margin-right: 3px;
}

.validation-message.ok {
    display: inline;
    color: green;
}

.validation-message.ok:before {
    display: inline-block;
    width: 16px;
    height: 16px;
    content: url("img/ok.png");
    vertical-align: -15%;
    margin-left: 3px;
    margin-right: 3px;
}

.validation-message.error {
    display: inline;
    color: red;
}

.validation-message.error:before {
    display: inline-block;
    width: 16px;
    height: 16px;
    content: url("img/error.png");
    vertical-align: -15%;
    margin-left: 3px;
    margin-right: 3px;
}

#error-message {
    margin: 10px auto;
    border: 1px solid maroon;
    border-radius: 5px;
    padding: 10px 20px;
    box-sizing: border-box;
    background-color: #ffcccc;
    display: none;
}

#error-message h1 {
    color: maroon;
    font-size: 120%;
    margin: 0;
    text-align: left;
}

#error-message pre {
    color: maroon;
    margin: 0;
    padding: 0;
    border: none;
    background-color: transparent;
}

footer {
    text-align: center;
    font-size: x-small;
    margin-top: -10px;
}

    </style>
  </head>

  <body>
    <h1>
      <img src="img/uhcal.png" alt="" /> UHasselt Calendar
    </h1>
  </body>

  <form id="name-input">
    <label for="calendar-name">
      Calendar name:
    </label>
    <input type="text" name="calendar-name" id="calendar-name" value=""
`         class="inputbox" placeholder="Calendar name..."
          onkeyup="checkcalname(this)"/>
  </form>

  <div id="name-results">

    <div class="warningbox" id="result-box-1">
      Calendar does not exist. A new one will be created.
    </div>

    <div class="warningbox" id="result-box-2">
      Calendar already exists. It will be overwritten.
    </div>

    <div class="errorbox" id="result-box-3">
      Filename may only contain alphanumeric (a-z, A-Z, 0-9) characters.
    </div>

  </div>

  <hr/>

  <div id="textfield">
    <a href="#" id="load-file">Load sample file</a>
    <textarea id="textarea" class="inputbox"></textarea>
    <div id="textfield-button">
      <button class="btn"><i class="icon-folder-open"></i> Load</button>
      <button class="btn"><i class="icon-hdd"></i> Save</button>
      <button class="btn"><i class="icon-ok"></i> Check</button>
      <span class="validation-message ok">OK!</span>
      <button class="btn float-right"><i class="icon-download"></i> Download</button>
      <button class="btn float-right"><i class="icon-upload"></i> Upload</button>
    </div>
  </div>

  <div id="error-message">
    <h1>Error</h1>
    <pre>stuff</pre>
  </div>

  <hr/>

  <footer>
    Copyright &copy; 2013 <a href="http://seysayux.net">Frank Erens</a>
  </footer>

  <script type="text/javascript" charset="utf-8">
      function displayCalendarNameResult(number) {
        switch(number) {
            case 0:
                $("#result-box-1").hide("blind", {}, 500);
                $("#result-box-2").hide("blind", {}, 500);
                $("#result-box-3").hide("blind", {}, 500);
                break;
            case 1:
                $("#result-box-1").show("blind", {}, 500);
                $("#result-box-2").css({ display: "none" });
                $("#result-box-3").css({ display: "none" });
                break;
            case 2:
                $("#result-box-1").css({ display: "none" });
                $("#result-box-2").show("blind", {}, 500);
                $("#result-box-3").css({ display: "none" });
                break;
            case 3:
                $("#result-box-1").css({ display: "none" });
                $("#result-box-2").css({ display: "none" });
                $("#result-box-3").show("blind", {}, 500);
                break;
          }
      }

      function checkcalname(input) {
          var val = $("#calendar-name").val();

          if(val == "") {
              displayCalendarNameResult(0);
          } else {
              $.ajax({
                  url: "?ajax=1&action=checkname&file="+encodeURIComponent(val)
              }).done(function(data) {

                  console.log(data);
                  if(data == "R_CHECKFN_NEXISTS") {
                      displayCalendarNameResult(1);
                  } else if(data == "R_CHECKFN_EXISTS"){
                      displayCalendarNameResult(2);
                  } else {
                      displayCalendarNameResult(3);
                  }

              });
          }
      }
  </script>

</html><?php
} else {

    error_reporting(E_ALL);
    ini_set('display_errors', '1');

    if(!isset($_GET["action"])) {
        echo "E_ACTION";
        exit();
    }

    function checkfilename($name) {
        if(preg_match("/^\\w+$/", $name)) {
            if(file_exists("$name.xml")) {
                return "R_CHECKFN_EXISTS";
            } else {
                return "R_CHECKFN_NEXISTS";
            }
        } else {
            return "R_CHECKFN_INVALID";
        }
    }

    function load($name) {
        $result = checkfilename($name);
        if($result != "R_CHECKFN_EXISTS") {
            echo $result;
            exit(1);
        }

        return file_get_contents("$name.xml");
    }

    function save($name, $data) {
        $check = check($data);
        if($check != "R_CHECK_OK") return $check;
        if(file_put_contents("$name.xml", $data) === FALSE) {
            return "E_NOWRITE";
        } else {
            return "R_SAVE_OK";
        }
    }

    function check($data) {
        $fname = tempnam("/tmp").".xml";
        if(file_put_contents($tempnam, $data) === FALSE) {
            return "E_NOWRITE";
        }

        $fname = "frankerens.xml";

        $result = shell_exec(
            "xmllint --noout --schema calendar.xsd $fname 2>&1");

        unlink($fname);

        $validates = strpos("$result", "validates");

        if($validates != FALSE && $validates < strpos("$result", "\n")) {
            return "R_CHECK_OK";
        } else {
            return $result;
        }
    }

    function checkfileset() {
        if(!isset($_GET["file"])) {
            echo "E_NOFILE";
            exit();
        }

        $_GET["file"] = urldecode($_GET["file"]);
    }

    function checkdataset() {
        if(!isset($_POST["data"])) {
            echo "E_NODATA";
            exit();
        }
    }
    switch($_GET["action"]) {
        case "load":
            checkfileset();
            header('Content-type: text/xml');
            echo load($_GET["file"]);
            break;

        case "save":
            checkdataset();
            checkfileset();
            echo save($_GET["file"], $_GET["data"]);
            break;

        case "check":
            checkdataset();
            echo check($_GET["data"]);
            break;

        case "checkname":
            checkfileset();
            echo checkfilename($_GET["file"]);
            break;

        case "download":
            header('Content-type: application/octet-stream');
            checkdataset();
            echo $_POST["data"];
            break;

        default:
            echo "E_ACTION";
    }
}
?>
