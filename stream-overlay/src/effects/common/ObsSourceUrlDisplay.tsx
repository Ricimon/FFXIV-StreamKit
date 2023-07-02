import { queryString } from "../../constants";

function ObsSourceUrlDisplay() {
  const searchParams = new URLSearchParams(window.location.search);
  searchParams.set(queryString.forObs, 'true');
  var url = window.location.origin + window.location.pathname + '?' + searchParams.toString();

  function handleFocus(event: React.FocusEvent<HTMLInputElement>) {
    event?.target?.select();
  }

  return (
    <div className='obs-url'>
      <label>OBS Source URL</label>
      <input type='text' value={url} onFocus={handleFocus} readOnly></input>
    </div>
  )
}

export default ObsSourceUrlDisplay;