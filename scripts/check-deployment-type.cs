//@req(next)

var lang = '';
nodes = jelastic.env.control.GetEnvInfo('${env.envName}', session).nodes
addon = 'unknown'
for (i = 0; i < nodes.length; i++){
  if (nodes[i].nodeGroup == 'cp') {
    type = nodes[i].engineType || (nodes[i].activeEngine || {}).type;
    addon = type ? (type == 'java' ? 'maven' : 'vcs') : 'mount'
      
    if (addon == 'mount') return {result:99, error: 'deploy to custom containers is not implemented yet', type: 'warning'}
    
    resp = {
      result: 0, 
      onAfterReturn: {}
    }
    resp.onAfterReturn[next] = {addon: addon, type: type};
    resp.onAfterReturn = [resp.onAfterReturn];
    type == 'java' ? lang = '${this.javaLang}' : lang = '${this.otherLangs}'; 
    
    resp.onAfterReturn.push({
      "showMessage": {
        lang: lang
      }
    });
    return resp
  }
}
return {result: 99, error: 'nodeGroup [cp] is not present in the topology', type: 'warning'}
