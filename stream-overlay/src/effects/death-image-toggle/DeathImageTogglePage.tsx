import { ChangeEvent, useEffect, useState } from 'react';
import styled from 'styled-components';
import { useInjection } from 'inversify-react';

import defaultAliveImage from './alive.png';
import defaultDeadImage from './dead.png';

import '../common/effects-common.css';
import Logger from '../../log/logDispatcher';
import { IWebsocketClient } from '../../websocket/websocketClientInterface';
import ObsSourceUrlDisplay from '../common/ObsSourceUrlDisplay';
import { queryString } from '../../constants';
import Character from '../common/character';

const componentId = 'death-image-toggle';
const aliveImageDataKey = componentId + '/alive-image';
const deadImageDataKey = componentId + '/dead-image';

const imageMimeType = /image\/(png|jpg|jpeg|gif)/i;

const Button = styled.button`
  margin: 0px 0.5rem;
`;

function DeathImageTogglePage() {
  const log = useInjection(Logger);
  const ws = useInjection(IWebsocketClient.$);
  const urlSearchParams = useInjection(URLSearchParams);
  const character = useInjection(Character);

  const [isAlive, setAliveState] = useState<boolean>(true);
  const [aliveImage, setAliveImage] = useState<string | null>(null);
  const [deadImage, setDeadImage] = useState<string | null>(null);

  useEffect(() => {
    var aliveImageSub = ws.listenForDataUpdate(aliveImageDataKey)
      .subscribe(data => setAliveImage(data ?? defaultAliveImage));
    var deadImageSub = ws.listenForDataUpdate(deadImageDataKey)
      .subscribe(data => setDeadImage(data ?? defaultDeadImage));

    var aliveStateSub = character.isAliveObservable.subscribe(setAliveState);

    return () => {
      aliveImageSub.unsubscribe();
      deadImageSub.unsubscribe();
      aliveStateSub.unsubscribe();
    }
  }, []);

  function handleAliveImageSelection(e: ChangeEvent<HTMLInputElement>) {
    handleImageSelection(e, aliveImageDataKey);
  }

  function handleDeadImageSelection(e: ChangeEvent<HTMLInputElement>) {
    handleImageSelection(e, deadImageDataKey);
  }

  function handleImageSelection(e: ChangeEvent<HTMLInputElement>, imageDataKey: string) {
    const file = e?.target?.files?.[0];

    if (!file) {
      return;
    }
    if (!file.type.match(imageMimeType)) {
      log.error('Image mime type is not valid.');
      return;
    }

    log.trace(`Picked file of type ${file.type}`);

    const fileReader = new FileReader()
    fileReader.onload = (event) => {
      const contents = event?.target?.result;
      if (!contents || typeof contents !== 'string') {
        log.error('Selected file could not be read.');
        return;
      }
      ws.pushData(imageDataKey, contents);
    };
    fileReader.onerror = (event) => {
      log.error(`Error on reading selected file: ${event?.target?.error}`)
    };
    fileReader.readAsDataURL(file);
  }

  function setAlive() {
    setAliveState(true);
    log.trace('Test alive state');
  }

  function setDead() {
    setAliveState(false);
    log.trace('Test dead state');
  }

  const imageSource = aliveImage && deadImage ?
    <img src={isAlive ? aliveImage : deadImage} /> :
    <p>Loading...</p>;

  let contents = (
    <div className='media'>
      {imageSource}
    </div>
  );
  const forObs = urlSearchParams.get(queryString.forObs) == 'true';
  if (!forObs) {
    contents = (
      <div>
        <h2>Death image toggle</h2>
        {contents}

        <div className='effect-config'>
          <div className='effect-config-section'>
            <div className='image-selection'>
              <label htmlFor='file'>Select alive image</label>
              <input type='file' accept='image/*' onChange={handleAliveImageSelection} />
            </div>
            <div className='image-selection'>
              <label htmlFor='file'>Select dead image</label>
              <input type='file' accept='image/*' onChange={handleDeadImageSelection} />
            </div>
          </div>

          <div className='effect-config-section'>
            <Button onClick={setAlive}>Test Alive</Button>
            <Button onClick={setDead}>Test Dead</Button>
          </div>

          <div className='effect-config-section'>
            <ObsSourceUrlDisplay />
          </div>
        </div>
      </div>
    );
  }

  return contents;
}

DeathImageTogglePage.path = '/' + componentId;

export default DeathImageTogglePage;