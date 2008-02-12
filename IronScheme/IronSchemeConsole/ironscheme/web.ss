(library (ironscheme web)
  (export
    method
    querystring
    form
    header
    request
    session
    session-set!
    user-agent
    url-decode
    url-encode
    html-decode
    html-encode
    map-path
    )
  (import (ironscheme)
    (ironscheme clr))

  (clr-reference system.web)
  (clr-using system.web)
  (clr-using system.collections.specialized)
  (clr-using system.web.sessionstate)
  
  (define (current-context)
    (clr-static-prop-get httpcontext current))
    
  (define (request)
    (clr-prop-get httpcontext request (current-context)))    
  
  (define (method)
    (clr-prop-get httprequest httpmethod (request)))    
  
  (define (get-querystring)
    (clr-prop-get httprequest querystring (request)))

  (define (get-form)
    (clr-prop-get httprequest form (request)))
    
  (define (get-headers)
    (clr-prop-get httprequest headers (request)))
    
    
  (define (nv-helper instance key)
    (define k (clr-indexer-get namevaluecollection instance (clr-cast system.string key)))
    (if (null? k) #f
        k))       

  (define (querystring key)
    (nv-helper (get-querystring) key))

  (define (form key)
    (nv-helper (get-form) key))
    
  (define (header key)
    (nv-helper (get-headers) key))
    
  (define (get-session)
    (clr-prop-get httpcontext session (current-context)))    

  (define (session key)
    (define k (clr-indexer-get httpsessionstate (get-session) (clr-cast system.string key)))
    (if (null? k) #f
        k))       
  
  (define (session-set! key value)
    (clr-indexer-set! httpsessionstate (get-session) (clr-cast system.string key) value)
    (void))
    
  (define (user-agent)
    (clr-prop-get httprequest useragent (request)))
    
  (define (server-util)
    (clr-prop-get httpcontext server (current-context)))
    
  (define (url-encode s)
    (clr-call httpserverutility urlencode (server-util) s))    
    
  (define (url-decode s)
    (clr-call httpserverutility urldecode (server-util) s))    

  (define (html-encode s)
    (clr-call httpserverutility htmlencode (server-util) s))    
    
  (define (html-decode s)
    (clr-call httpserverutility htmldecode (server-util) s))    
    
  (define (map-path p)
    (clr-call httpserverutility mappath (server-util) p))   
  
  
  
  (clr-clear-usings)
    
)