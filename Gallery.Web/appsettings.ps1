function Get-Service-Name {
    return "app.gallery.web"
}

function Get-Executable-Name { 
    return "Gallery.Web.exe" 
}

function Get-Description {
    return "Gallery http://localhost:62492";
}

function Get-Publish-Folder {
    $serviceName = Get-Service-Name
    return "C:\applications\_publish_collections\" + $serviceName
}